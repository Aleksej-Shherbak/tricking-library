const initState = () => ({
  tricks: [],
  categories: [],
  difficulties: [],
  isTrickDialogOpen: false,
  isCategoryDialogOpen: false,
  isDifficultiesDialogOpen: false
});

export const state = initState;

export const getters = {
  trickById: state => id => state.tricks.find(x => x.id === id),
  trickItems: state => state.tricks.map(({ name, id }) => ({
    text: name,
    value: id
  })),
  categoryItems: state => state.categories.map(({ name, id }) => ({
    text: name,
    value: id
  })),
  difficultyItems: state => state.difficulties.map(({ name, id }) => ({
    text: name,
    value: id
  })),
};

export const mutations = {
  reset(state) {
    Object.assign(state, initState());
  },
  setTricks(state, { tricks }) {
    state.tricks = tricks;
  },
  setCategories(state, { categories }) {
    state.categories = categories;
  },
  setDifficulties(state, { difficulties }) {
    state.difficulties = difficulties;
  },
  toggleTrickDialogActivity(state) {
    state.isTrickDialogOpen = !state.isTrickDialogOpen;
  },
  toggleCategoryDialogActivity(state) {
    state.isCategoryDialogOpen = !state.isCategoryDialogOpen;
  },
  toggleDifficultiesDialogActivity(state) {
    state.isDifficultiesDialogOpen = !state.isDifficultiesDialogOpen;
  }
}

export const actions = {
  async fetchTricks({ commit }) {
    const tricks = await this.$axios.$get('/api/tricks');
    const difficulties = await this.$axios.$get('/api/difficulties');
    const categories = await this.$axios.$get('/api/categories');

    console.log(tricks, difficulties, categories);
    commit('setTricks', { tricks: tricks  });
    commit('setCategories', { categories: categories });
    commit('setDifficulties', { difficulties: difficulties });
  },

  async createTrick({commit, dispatch}, { trick }) {
    await this.$axios.$post('/api/tricks', trick);
    await dispatch('fetchTricks');
  }
}
