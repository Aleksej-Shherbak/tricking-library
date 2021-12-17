<template>
  <v-dialog @click:outside="closeTrickDialog" :value="isTrickDialogOpen">

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
      <div class="d-flex justify-space-between">
        <v-btn @click="closeTrickDialog"> <v-icon>mdi-close</v-icon>&nbsp; Close</v-btn>
        <v-btn class="mr-4" @click="saveTrick" :disabled="!cleanedTrickName">Submit</v-btn>
      </div>
    </form>
  </v-dialog>
</template>

<script>
import { mapActions, mapMutations, mapState } from 'vuex';

export default {
  name: "trick-form",
  data: () => {
    return {
      trickName: '',
    }
  },
  computed: {
    ...mapState('tricks', ['isTrickDialogOpen']),
    cleanedTrickName() {
      return this.trickName.trim();
    }
  },
  methods: {
    ...mapActions('tricks', ['createTrick']),
    ...mapMutations('tricks', ['toggleTrickDialogActivity']),
    async saveTrick() {
      await this.createTrick({ trick: { name: this.trickName } })
      this.closeTrickDialog();
    },
    closeTrickDialog() {
      this.toggleTrickDialogActivity();
      this.trickName = '';
    }
  }
}
</script>

<style scoped>
</style>
