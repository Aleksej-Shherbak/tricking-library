<template>
  <div class="d-flex justify-center align-start">
      <div v-if="this.tricks[categoryId]" class="mx-2">
        <div v-for="t in this.tricks[categoryId]">
          {{ t.name }}
          <div>
            <video controls="controls" width="400" height="300"
                   :src="`http://localhost:5000/api/videos/${t.video}`"></video>
          </div>
        </div>
      </div>
      <div v-else>
        There are no tricks
      </div>

    <div class="mx-2 sticky">
      Category: {{ categoryId }}
    </div>
  </div>
</template>

<script>
import { mapState } from "vuex";

export default {
  computed: {
    categoryId: function (){
      return this.$route.params.category;
    },
    ...mapState('tricks', ['tricks']),
  },
    async fetch() {
      const categoryId = this.categoryId;
      await this.$store.dispatch('tricks/fetchTricksInCategory', { categoryId })
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
