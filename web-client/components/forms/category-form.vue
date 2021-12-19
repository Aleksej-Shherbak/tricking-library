<template>
  <v-dialog @click:outside="toggleCategoryDialogActivity" max-width="700" :value="isCategoryDialogOpen">

    <template v-slot:activator="{ on }">
      <v-btn depressed @click="toggleCategoryDialogActivity">
        Create category
      </v-btn>
    </template>
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

          <div class="d-flex justify-space-between">
            <v-btn @click="toggleCategoryDialogActivity"> <v-icon>mdi-close</v-icon>&nbsp; Close</v-btn>
            <v-btn @click="save">Save</v-btn>
          </div>
        </form>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script>
import { mapState, mapMutations, mapGetters } from "vuex";

export default {
  name: "category-form",
  computed: {
    ...mapState('tricks', ['isCategoryDialogOpen']),
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
    ...mapMutations('tricks', ['toggleCategoryDialogActivity']),
    save() {
      this.$axios.$post('/api/categories', this.form);
    },
  }
}
</script>

<style scoped>

</style>
