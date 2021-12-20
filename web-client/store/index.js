const initState = () => ({});

export const state = initState;

export const mutations = {

  reset(state) {
    Object.assign(state, initState());
  }
}

export const actions = {
  nuxtServerInit({commit, dispatch}) {
    return dispatch('tricks/fetchTricks');
  }
}
