const initState = () => ({
  categories: [],
  isCategoryDialogOpen: false
});

export const state = initState;

export const mutations = {
  reset(state) {
    Object.assign(state, initState());
  },
  setCategories(state, { categories }) {
    state.categories = categories;
  },
  toggleCategoryDialogActivity(state) {
    state.isCategoryDialogOpen = !state.isCategoryDialogOpen;
  }
}

export const actions = {
  async fetchCategories ({ commit }) {
    const result = await this.$axios.$get('/api/categories');
    commit('setCategories', result);
  },
  async createCategory({commit, dispatch}, category) {
    await this.$axios.$post('/api/categories', category);
    await dispatch('fetchCategories');
  }
}
