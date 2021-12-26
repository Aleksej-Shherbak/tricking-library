const initState = () => ({
  tricks: [],
  categories: [],
  difficulties: [],
});

export const state = initState;

export const getters = {
  difficultyById: state => id => state.difficulties.find(x => x.id === id),
  trickById: state => id => state.tricks.find(x => x.id === id),
  categoryById: state => id => state.categories.find(x => x.id === id),
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
}

export const actions = {
  async fetchTricks({ commit }) {
    const tricks = await this.$axios.$get('/api/tricks');
    const difficulties = await this.$axios.$get('/api/difficulties');
    const categories = await this.$axios.$get('/api/categories');

    commit('setTricks', { tricks: tricks  });
    commit('setCategories', { categories: categories });
    commit('setDifficulties', { difficulties: difficulties });
  },

  async createTrick({commit, dispatch}, { trick }) {
    await this.$axios.$post('/api/tricks', trick);
    await dispatch('fetchTricks');
  }
}
