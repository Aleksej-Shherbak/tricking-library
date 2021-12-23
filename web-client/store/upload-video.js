const initState = () => ({
  video: null,
  objectUrl: null,
});

export const state = initState;

export const getters = {
  previewUrl : (state) => {
    return state.objectUrl;
  },
  video : (state) => {
    return state.video;
  }
}

export const mutations = {
  setVideo(state, video) {
    console.log(video)
    if (video) {
      state.video = video;
      state.objectUrl = URL.createObjectURL(state.video);
    }
  },
  disposeVideo (state) {
    if (state.objectUrl) {
      URL.revokeObjectURL(state.objectUrl);
      Object.assign(state, initState());
    }
  }
}
