<template>
  <div>
    Hello moderation
    <v-btn v-for="i in items" :key="i.id" :to="`/moderation/${i.type}/${i.target}`">
      {{ i.target }}
    </v-btn>
  </div>
</template>

<script>
export default {
  data: () => ({
    items: []
  }),
  async fetch() {
    this.items = await this.$axios.$get('/api/moderationItems');
  }
}
</script>

<style scoped>

</style>
