<template>
  <v-dialog @click:outside="closeCategoryDialog" :value="isCategoryDialogOpen">
    <form class="pa-6">
      <v-text-field
        v-model="categoryName"
        :counter="40"
        maxlength="40"
        label="Category name"
        required
        error-count="1"
      ></v-text-field>
      <v-btn class="mr-4" @click="saveCategory" :disabled="!cleanedCategoryName">Submit</v-btn>
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
