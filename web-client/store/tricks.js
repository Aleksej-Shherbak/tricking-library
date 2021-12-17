const initState = () => ({
  tricks: [],
  isTrickDialogOpen: false
});

export const state = initState;

export const mutations = {
  reset(state) {
    Object.assign(state, initState());
  },
  setTricks(state, { tricks }) {
    state.tricks = tricks;
  },
  toggleTrickDialogActivity(state) {
    state.isTrickDialogOpen = !state.isTrickDialogOpen;
  }
}

export const actions = {
  async fetchTricks({ commit }) {
    const result = await this.$axios.$get('/api/tricks');
    commit('setTricks', { tricks: result });
  },

  async createTrick({commit, dispatch}, { trick }) {
    await this.$axios.$post('/api/tricks', trick);
    await dispatch('fetchTricks');
  }
}
