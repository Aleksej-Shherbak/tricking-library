<template>
  <v-card>
    <v-card-title>Trick category</v-card-title>
    <v-card-text>
      <form class="pa-6">
        <v-text-field
          class="mb-3"
          v-model="form.name"
          :counter="40"
          maxlength="40"
          label="Category name"
          required
        ></v-text-field>

        <v-text-field
          class="mb-3"
          v-model="form.description"
          :counter="300"
          maxlength="300"
          label="Category description"
          required
        ></v-text-field>

          <v-btn @click="save">Save</v-btn>
      </form>
    </v-card-text>
  </v-card>
</template>

<script>
export default {
  name: "category-form",
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
      this.$axios.$post('/api/categories', this.form);
      this.resetForm();
    },
  }
}
</script>

<style scoped>

</style>
