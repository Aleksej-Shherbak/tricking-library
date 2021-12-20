<template>
  <div class="d-flex justify-center align-start">
      <div v-if="submissions[trickId]" class="mx-2">
        <div v-for="t in submissions[trickId]">
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

    <v-sheet class="pa-3 ma-2 sticky">
      <div class="text-h5"> <span >Trick: {{ trick.name }}</span>
        <v-chip class="mb-1 ml-2" :to="`/difficulty/${difficulty.id}`" small>
          {{ difficulty.name }} difficulty
        </v-chip>
      </div>
      <v-divider class="my-1"></v-divider>
      <div class="text-body-2"> {{ trick.description }}</div>
      <v-divider class="my-1"></v-divider>
      <div v-for="rd in relatedData" v-if="rd.data.length > 0">
        <div class="text-subtitle-1">{{ rd.title }}:</div>
        <v-chip-group>
          <v-chip v-for="d in rd.data" :key="rd.idFactory(d)" small :to="rd.routeFactory(d)">
            {{ d.name }}
          </v-chip>
        </v-chip-group>
      </div>
    </v-sheet>
  </div>
</template>

<script>
import {mapGetters, mapState} from "vuex";

export default {
  data:() => ({
    trick: null,
    difficulty: null,
  }),
  computed: {
    ...mapState('submissions', ['submissions']),
    ...mapState('tricks', ['categories', 'tricks']),
    ...mapGetters('tricks', ['trickById', 'difficultyById']),
    trickId: function () {
      return this.$route.params.trick;
    },
    relatedData() {
      return [
        {
          title : 'Categories',
          data : this.categories.filter(({ id }) => this.trick.categories.includes(id)),
          idFactory: c => `category-${c.id}`,
          routeFactory: c => `/category/${c.id}`,
        },
        {
          title : 'Prerequisites',
          data : this.tricks.filter(({ id }) => this.trick.prerequisites.includes(id)),
          idFactory: t => `trick-${t.id}`,
          routeFactory: t => `/trick/${t.id}`,
        },
        {
          title : 'Progressions',
          data : this.tricks.filter(({ id }) => this.trick.progression.includes(id)),
          idFactory: t => `trick-${t.id}`,
          routeFactory: t => `/trick/${t.id}`,
        },
      ];
    }
  },
  async fetch() {
    const trickId = this.trickId;
    this.trick = this.trickById(trickId);
    this.difficulty = this.difficultyById(this.trick.difficulty);

    await this.$store.dispatch('submissions/fetchSubmissionsForTrick', { trickId })
  },
  head() {
    // meta data for the page is here ...
    if (!this.trick) {
      return {};
    }
    return {
      title: this.trick.name,
      meta: [
        {
          hid: 'description',
          name: 'description',
          content: this.trick.description
        }
      ]
    };
  }
}
</script>

<style scoped>

</style>
