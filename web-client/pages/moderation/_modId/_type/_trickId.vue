<template>
  <div>
    <div v-if="item">{{ item.description }}</div>
    <div v-if="replyId > 0">
      Replying to {{ replyId }} <v-btn @click="replyId = 0">Clear</v-btn>
    </div>
    <div>
      <v-text-field label="Comment" v-model="comment"></v-text-field>
      <v-btn @click="send">Send</v-btn>
    </div>
    <div class="my-1" v-for="c in comments" >
      <span v-html="c.htmlContent"></span>
      <v-btn @click="replyId = c.id">Reply</v-btn>
      <v-btn @click="loadReplies(c.id)">Load replyes</v-btn>
      <div v-for="r in c.replies">
        <span v-html="r.htmlContent"></span>
      </div>
    </div>
  </div>
</template>

<script>
// TODO fix all the magic
  const endpointResolver = (type) => {
      if (type === 'trick') {
        return 'tricks'
      }
  }

  const commentWithReplies = comment => ({
    ...comment,
    replies: []
  });

export default {
  data: () => ({
    item: null,
    comments: [],
    comment: '',
    replyId: 0,
  }),
  created() {
    const { modId, type, trickId } = this.$route.params;
    const endpoint = endpointResolver(type);

    this.item = this.$axios.$get(`/api/${endpoint}/${trickId}`)
    .then((item) => this.item = item);

    this.item = this.$axios.$get(`/api/moderationItems/${modId}/comments`)
    .then((comments) => this.comments = comments.map(commentWithReplies));
  },
  methods: {
    send() {
      const { modId } = this.$route.params;

      if (this.replyId > 0) {
        this.item = this.$axios.$post(`/api/comments/${this.replyId}/replies`, {
          content: this.comment
        }).then((comment) => this.comments.find(({ id }) => id === this.replyId).replies.push(comment));

      } else {
        this.item = this.$axios.$post(`/api/moderationItems/${modId}/comment`, {
          content: this.comment
        }).then((comment) => this.comments.push(commentWithReplies(comment)));
      }
    },
    loadReplies(commentId) {
      this.item = this.$axios.$get(`/api/comments/${commentId}/replies`)
        .then((replies) => {
          let comment = this.comments.find(({ id }) => id === commentId);
          this.$set(comment, 'replies', replies);
        });

    }
  }
}
</script>

<style scoped>

</style>
