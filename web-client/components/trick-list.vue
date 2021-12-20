<template>
  <div>
    <v-text-field label="Search" placeholder="e.g. cork/flip/kick" outlined prepend-inner-icon="mdi-magnify" v-model="filter"></v-text-field>
    <div v-for="t in filteredTricks">
      <p>NAME: {{ t.name }}</p>
      <p>DESCRIPTION: {{ t.description }}</p>
      <v-divider></v-divider>
    </div>
  </div>
</template>

<script>
import {mapGetters} from "vuex";

export default {
  name: "trick-list",
  props: {
    tricks: {
      required: true,
      type: Array,
    }
  },
  data: () => ({
    filter: '',
  }),
  computed: {
    ...mapGetters('tricks', ['difficultyById']),

    filteredTricks : function() {
      if (!this.filter) {
        return this.tricks
      }
      const sanitizedFilterRequest = this.filter.trim().toLowerCase();
      return this.tricks.filter(t => t.name.toLowerCase().includes(sanitizedFilterRequest) ||
        t.description.toLowerCase().includes(sanitizedFilterRequest));
    }
  }
}
</script>

<style scoped>

</style>
