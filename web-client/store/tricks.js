const initState = () => ({
  tricks: {},
  isUploadPopupOpened: false
});

export const state = initState;

export const mutations = {
  setTricks(state, { tricks, categoryId }) {
    let newTricks = {};
    newTricks[categoryId] = tricks;
    state.tricks = { ...state.tricks, ...newTricks };
  },
  reset(state) {
    Object.assign(state, initState());
  },
  toggleUploadTrickActivity (state) {
    state.isUploadPopupOpened = !state.isUploadPopupOpened;
  }
}

export const actions = {
  async createTrick({ commit, dispatch }, { trickFormData } ) {
    await this.$axios.$post('/api/tricks', trickFormData);
  },
  async fetchTricksInCategory ({ commit, state }, { categoryId }) {
    if (state.tricks[categoryId] !== undefined) {
      return;
    }

    const result = await this.$axios.$get(`/api/categories/${categoryId}/tricks`);
    commit('setTricks', { categoryId, tricks: result });
  },
}
