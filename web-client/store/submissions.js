const initState = () => ({
  submissions: {},
});

export const state = initState;

export const mutations = {
  setSubmissions(state, { submissions, trickId }) {
    let newSubmission = {};
    newSubmission[trickId] = submissions;
    state.submissions = { ...state.submissions, ...newSubmission };
  },
  reset(state) {
    Object.assign(state, initState());
  }
}

export const actions = {
  async createSubmission({ commit, dispatch }, { submissionFormData } ) {
    await this.$axios.$post('/api/submissions', submissionFormData);
  },

  async fetchSubmissionsForTrick ({ commit, state }, { trickId }) {
    if (state.submissions[trickId] !== undefined) {
      return;
    }
    const result = await this.$axios.$get(`/api/tricks/${trickId}/submissions`);
    commit('setSubmissions', { trickId, submissions: result });
  },
}
