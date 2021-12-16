<template>
  <div>
    <div>
      Category: {{ categoryId }}
    </div>

    <div v-if="this.tricks[categoryId]" class="mb-3">
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

</style>
