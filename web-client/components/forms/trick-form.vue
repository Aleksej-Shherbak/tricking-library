<template>
  <v-dialog @click:outside="closeTrickDialog" max-width="700" :value="isTrickDialogOpen">

    <template v-slot:activator="{ on }">
      <v-btn depressed @click="toggleTrickDialogActivity">
        Create trick
      </v-btn>
    </template>

    <form class="pa-6">
      <v-text-field
        class="mb-3"
        v-model="trickName"
        :counter="40"
        maxlength="40"
        label="Trick name"
        required
        error-count="1"
      ></v-text-field>

      <v-text-field
        class="mb-3"
        v-model="description"
        :counter="300"
        maxlength="300"
        label="Trick description"
        required
        error-count="1"
      ></v-text-field>

      <v-select :items="difficultyItems" label="Difficulty" v-model="difficulty" outlined
                 chips deletable-chips></v-select>

      <v-select :items="trickItems" label="Prerequisites" v-model="prerequisites" outlined
                multiple chips deletable-chips></v-select>

      <v-select :items="trickItems" label="Progressions" v-model="progressions" outlined
                multiple chips deletable-chips></v-select>

      <v-select :items="categoryItems" label="Categories" v-model="categories" outlined
                multiple chips deletable-chips></v-select>


      <div class="d-flex justify-space-between">
        <v-btn @click="closeTrickDialog"> <v-icon>mdi-close</v-icon>&nbsp; Close</v-btn>
        <v-btn class="mr-4" @click="saveTrick" :disabled="!cleanedTrickName">Submit</v-btn>
      </div>
    </form>
  </v-dialog>
</template>

<script>
import {mapActions, mapGetters, mapMutations, mapState} from 'vuex';

export default {
  name: "trick-form",
  data: () => {
    return {
      trickName: '',
      description: '',
      difficulty: '',
      prerequisites: [],
      progressions: [],
      categories: [],
    }
  },
  computed: {
    ...mapState('tricks', ['isTrickDialogOpen']),
    ...mapGetters('tricks', ['categoryItems', 'difficultyItems', 'trickItems']),
    cleanedTrickName() {
      return this.trickName.trim();
    }
  },
  methods: {
    ...mapActions('tricks', ['createTrick']),
    ...mapMutations('tricks', ['toggleTrickDialogActivity']),
    async saveTrick() {
      await this.createTrick({ trick: {
          name: this.trickName,
          description: this.description,
          difficulty: this.difficulty,
          categories: this.categories,
        }
      })
      this.closeTrickDialog();
    },
    closeTrickDialog() {
      this.toggleTrickDialogActivity();
      this.trickName = '';
      this.description = '';
      this.difficulty = '';
      this.prerequisites = [];
      this.progressions = [];
    }
  }
}
</script>

<style scoped>
</style>
