const initState = () => ({
  tricks: [],
  isUploadPopupOpened: false
});

export const state = initState;

export const mutations = {
  setTricks(state, { tricks }) {
    state.tricks = tricks;
  },
  reset(state) {
    Object.assign(state, initState());
  },
  toggleUploadTrickActivity (state) {
    state.isUploadPopupOpened = !state.isUploadPopupOpened;
  }
}

export const actions = {
  async fetchTricks({ commit }) {
    const tricks = await this.$axios.$get('/api/tricks');
    commit('setTricks', { tricks });
  },
  async createTrick({ commit, dispatch }, { trickFormData } ) {
    await this.$axios.$post('/api/tricks', trickFormData);
    await dispatch('fetchTricks');
  }
}
