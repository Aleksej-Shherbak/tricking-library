<template>
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

    <v-select :items="this.difficultyItems" label="Difficulty" v-model="difficulty" outlined
              chips deletable-chips></v-select>

    <v-select :items="this.trickItems" label="Prerequisites" v-model="prerequisites" outlined
              multiple chips deletable-chips></v-select>

    <v-select :items="this.trickItems" label="Progressions" v-model="progressions" outlined
              multiple chips deletable-chips></v-select>

    <v-select :items="this.categoryItems" label="Categories" v-model="categories" outlined
              multiple chips deletable-chips></v-select>

    <div class="d-flex justify-space-between">
      <v-btn class="mr-4" @click="saveTrick" :disabled="!cleanedTrickName">Submit</v-btn>
    </div>
  </form>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';

export default {
  name: "trick-form",
  props: {
    resetForm: {
      required: true,
      type: Function
    }
  },
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
    ...mapGetters('tricks', ['categoryItems', 'difficultyItems', 'trickItems']),
    cleanedTrickName() {
      return this.trickName.trim();
    }
  },
  methods: {
    ...mapActions('tricks', ['createTrick']),
    async saveTrick() {
      await this.createTrick({
        trick: {
          name: this.trickName,
          description: this.description,
          difficulty: this.difficulty,
          categories: this.categories,
        }
      })
      this.resetForm();
    },
  }
}
</script>

<style scoped>
</style>
