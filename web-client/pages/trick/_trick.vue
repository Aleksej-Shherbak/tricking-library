<template>
  <div class="d-flex justify-center align-start">
      <div v-if="this.submissions[trickId]" class="mx-2">
        <div v-for="t in this.submissions[trickId]">
          {{ t.name }}
          <div>
            <video controls="controls" width="400" height="300"
                   :src="`http://localhost:5000/api/videos/${t.video}`"></video>
          </div>
        </div>
      </div>
      <div v-else>
        There are no submissions for this trick
      </div>

    <div class="mx-2 sticky">
      Trick: {{ trickId }}
    </div>
  </div>
</template>

<script>
import { mapState } from "vuex";

export default {
  computed: {
    trickId: function (){
      return this.$route.params.trick;
    },
    ...mapState('submissions', ['submissions']),
  },
    async fetch() {
      const trickId = this.trickId;
      await this.$store.dispatch('submissions/fetchSubmissionsForTrick', { trickId })
    }
}
</script>

<style scoped>
  .sticky {
    position: sticky;
    position: -webkit-sticky;
    top: 12px;
  }
</style>
