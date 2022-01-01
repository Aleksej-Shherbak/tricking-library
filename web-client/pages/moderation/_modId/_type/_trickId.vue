<template>
  <div>
    <div v-if="item">{{ item.description }}</div>

    <v-row>
      <v-col cols="7">
        <comment-section :comments="comments" @send="sendComment" />
      </v-col>
      <v-col cols="5">
        <v-card class="pb-1">
        <!-- 3 is for now. TODO: change it later          -->
          <v-card-title>Reviews ({{ approveCount }} / 3)</v-card-title>
          <v-card-text>
            <div v-if="reviews.length > 0">
              <div v-for="review in reviews" :key="review.id">
                <v-icon :color="reviewStatusColor(review.status)">{{ reviewStatusIcon(review.status) }}</v-icon>
                Username
                <span v-if="review.comment">
                   - {{ review.comment }}
                </span>
              </div>
            </div>
            <div v-else>No reviews</div>

            <v-divider class="my-3"></v-divider>

            <v-text-field v-model="reviewComment" label="Review comment"> </v-text-field>
          </v-card-text>
          <v-card-actions class="justify-space-around">
            <v-btn v-for="action in reviewActions"
                   :key="action.title"
                   :color="action.color"
                   :disabled="action.isDisabled"
                  @click="createReview(action.status)">
              <v-icon>{{ action.icon }}</v-icon>{{ action.title }}
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>


  </div>
</template>

<script>
import CommentSection from '@/components/comments/comment-section';

// TODO fix all the magic
  const endpointResolver = (type) => {
      if (type === 'trick') {
        return 'tricks'
      }
  }

  const REVIEW_STATUS = {
    APPROVE : 0,
    REJECTED : 1,
    WAITING : 2,
  };

  const reviewStatusColor = (status) => {
    if (REVIEW_STATUS.APPROVE === status) {
      return 'green';
    }
    if (REVIEW_STATUS.REJECTED === status) {
      return 'red';
    }
    if (REVIEW_STATUS.WAITING === status) {
      return 'orange';
    }
    return '';
  }

  const reviewStatusIcon = (status) => {
    if (REVIEW_STATUS.APPROVE === status) {
      return 'mdi-check';
    }
    if (REVIEW_STATUS.REJECTED === status) {
      return 'mdi-close';
    }
    if (REVIEW_STATUS.WAITING === status) {
      return 'mdi-clock';
    }
    return '';
  }

export default {
  components: {
    CommentSection
  },
  data: () => ({
    item: null,
    comments: [],
    reviews: [],
    reviewComment: '',
    replyId: 0,
  }),
  created() {
    const { modId, type, trickId } = this.$route.params;
    const endpoint = endpointResolver(type);

    this.$axios.$get(`/api/${endpoint}/${trickId}`)
    .then((item) => this.item = item);

    this.$axios.$get(`/api/moderationItems/${modId}/comments`)
    .then((comments) => this.comments = comments);

    this.$axios.$get(`/api/moderationItems/${modId}/reviews`)
      .then((reviews) => this.reviews = reviews);
  },
  methods: {
    reviewStatusColor,
    reviewStatusIcon,
    sendComment(content) {
      const { modId } = this.$route.params;

     return this.$axios.$post(`/api/moderationItems/${modId}/comment`, {
        content
      }).then((comment) => this.comments.push(comment));
    },
    createReview(status) {
      const { modId } = this.$route.params;

      return this.$axios.$post(`/api/moderationItems/${modId}/reviews`, {
        comment: this.reviewComment,
        status: status
      }).then((review) => this.reviews.push(review));
    },
  },
  computed: {
    reviewActions() {
      return [
        {
          title: 'Approve', status: REVIEW_STATUS.APPROVE,
          icon: reviewStatusIcon(REVIEW_STATUS.APPROVE),
          color: reviewStatusColor(REVIEW_STATUS.APPROVE),
          isDisabled: false
        },
        {
          title: 'Reject',
          status: REVIEW_STATUS.REJECTED,
          icon: reviewStatusIcon(REVIEW_STATUS.REJECTED),
          color: reviewStatusColor(REVIEW_STATUS.REJECTED),
          isDisabled: !this.reviewComment.trim()
        },
        {
          title: 'Wait',
          status: REVIEW_STATUS.WAITING,
          icon: reviewStatusIcon(REVIEW_STATUS.WAITING),
          color: reviewStatusColor(REVIEW_STATUS.WAITING),
          isDisabled: !this.reviewComment.trim()
        },
      ]
    },
    approveCount() {
      return this.reviews.filter(({ status }) => status === REVIEW_STATUS.APPROVE).length;
    }
  }
}
</script>

<style scoped>

</style>
