const initState = () => ({
  user: null,
  isLoading: true,
});

export const state = initState;

const ROLES = {
  MODERATOR: 'Mod'
}

export const getters = {
  authenticated: state => !state.isLoading && state.user !== null,
  moderator: (state, getters) => getters.authenticated && state.user.profile.role === ROLES.MODERATOR,
}

export const mutations = {
  saveUser(state, { user }) {
    state.user = user;
  },
  finish(state) {
    state.isLoading = false;
  }
}

export const actions = {
  initialize({ commit }) {
    this.$auth.getUser()
      .then(user => {
        if (user) {
          commit('saveUser', { user });
          return  this.$axios.setToken(`Bearer ${user.access_token}`)
        }
      })
      // todo something about the errors
      .finally(() => commit('finish'));
  }
}
