﻿<template>
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
      <v-btn @click="save">Save</v-btn>
    </form>

</template>

<script>

export default {
  name: "difficulty-form",
  props: {
    resetForm: {
      required: true,
      type: Function
    }
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
    save() {
      this.$axios.$post('/api/difficulties', this.form);
      this.resetForm();
    },
  }
}
</script>

<style scoped>

</style>
