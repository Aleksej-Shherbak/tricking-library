<template>
  <v-dialog @click:outside="closeCategoryDialog" :value="isCategoryDialogOpen">

    <template v-slot:activator>
      <v-btn depressed @click="toggleCategoryDialogActivity">
        Create categpry
      </v-btn>
    </template>

    <form class="pa-6">
      <v-text-field
        class="mb-3"
        v-model="categoryName"
        :counter="40"
        maxlength="40"
        label="Category name"
        required
        error-count="1"
      ></v-text-field>
      <div class="d-flex justify-space-between">
        <v-btn @click="closeCategoryDialog"> <v-icon>mdi-close</v-icon>&nbsp; Close</v-btn>
        <v-btn class="mr-4" @click="saveCategory" :disabled="!cleanedCategoryName">Submit</v-btn>
      </div>
    </form>
  </v-dialog>
</template>

<script>
import { mapActions, mapMutations, mapState } from 'vuex';

export default {
  name: "category-form",
  data: () => {
    return {
      categoryName: '',
    }
  },
  computed: {
    ...mapState('category', ['isCategoryDialogOpen']),
    cleanedCategoryName() {
      return this.categoryName.trim();
    }
  },
  methods: {
    ...mapActions('category', ['createCategory']),
    ...mapMutations('category', ['toggleCategoryDialogActivity']),
    async saveCategory() {
      await this.createCategory({ category: { name: this.categoryName } })
      this.closeCategoryDialog();
    },
    closeCategoryDialog() {
      this.toggleCategoryDialogActivity();
      this.categoryName = '';
    }
  }
}
</script>

<style scoped>
</style>
