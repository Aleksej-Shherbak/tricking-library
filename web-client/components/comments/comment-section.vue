<template>
  <div>
    <comment-input @send="(c) => $emit('send', c)"></comment-input>

    <v-divider class="my-5"></v-divider>

    <comment v-for="comment in comments" :comment="comment" :key="comment.id"></comment>

  </div>
</template>

<script>
import  CommentInput from './comment-input';
import  Comment from './comment';

export default {
  name: "comment-section",
  components: { CommentInput, Comment },
  props: {
    comments: {
      type: Array,
      required: true,
    }
  }
}
</script>

<style scoped>

</style>
