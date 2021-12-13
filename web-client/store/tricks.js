const initState = () => ({
  tricks: []
});

export const state = initState;

export const mutations = {
  setTricks(state, { tricks }) {
    state.tricks = tricks;
  },
  reset(state) {
    Object.assign(state, initState());
  }
}

export const actions = {
  async fetchTricks({ commit }) {
    const tricks = await this.$axios.$get('http://localhost:5000/api/tricks');
    commit('setTricks', { tricks });
  },
  async createTrick({ commit, dispatch }, { trickFormData } ) {
    await this.$axios.$post('http://localhost:5000/api/tricks', trickFormData);
    await dispatch('fetchTricks');
  }
}
