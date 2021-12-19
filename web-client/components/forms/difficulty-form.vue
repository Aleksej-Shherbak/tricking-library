<template>
  <v-dialog @click:outside="toggleDifficultiesDialogActivity" max-width="700" :value="isDifficultiesDialogOpen">

    <template v-slot:activator="{ on }">
      <v-btn depressed @click="toggleDifficultiesDialogActivity">
        Create difficulty
      </v-btn>
    </template>
    <v-card>
      <v-card-title>Trick difficulty</v-card-title>
      <v-card-text>
        <form class="pa-6">
          <v-text-field
            class="mb-3"
            v-model="form.name"
            :counter="40"
            maxlength="40"
            label="Difficulty name"
            required
          ></v-text-field>

          <v-text-field
            class="mb-3"
            v-model="form.description"
            :counter="300"
            maxlength="300"
            label="Difficulty description"
            required
          ></v-text-field>

          <div class="d-flex justify-space-between">
            <v-btn @click="toggleDifficultiesDialogActivity"> <v-icon>mdi-close</v-icon>&nbsp; Close</v-btn>
            <v-btn @click="save">Save</v-btn>
          </div>
        </form>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script>
import {mapState, mapMutations, mapGetters} from "vuex";

export default {
  name: "difficulty-form",
  computed: {
    ...mapState('tricks', ['isDifficultiesDialogOpen']),
  },
  data: () => {
    return {
      form: {
        name: '',
        description: '',
      }
    };
  },
  methods: {
    ...mapMutations('tricks', ['toggleDifficultiesDialogActivity']),
    save() {
      this.$axios.$post('/api/difficulties', this.form);
    },
  }
}
</script>

<style scoped>

</style>
