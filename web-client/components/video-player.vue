<template>
  <div class="video-container">
    <div class="play-buttons" @click="isPlaying = !isPlaying">
      <v-icon size="78" v-if="!isPlaying">mdi-play</v-icon>
      <v-icon v-else class="pause" size="78">mdi-pause</v-icon>
    </div>
    <video ref="video" muted loop width="400" height="300"
           :src="`http://localhost:5000/api/videos/${video}`"></video>
  </div>
</template>

<script>
export default {
  // todo take care of the localhost address
  name: "video-player",
  props: {
    video: {
      required: true,
      type: String
    }
  },
  data: () => ({
    isPlaying: false
  }),
  watch: {
    isPlaying: function (value) {
      if (value) {
        this.$refs.video.play();
      } else {
        this.$refs.video.pause();
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.video-container {
  position: relative;
  width: 400px;
  display: flex;
  border-top-left-radius: inherit;
  border-top-right-radius: inherit;

  video {
    width: 100%;
    z-index: 1;
    border-top-left-radius: inherit;
    border-top-right-radius: inherit;
  }

  .play-buttons {
    background-color: rgba(0, 0, 0, 0.36);
    position: absolute;
    width: 100%;
    height: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 2;
    cursor: pointer;

    .pause {
      opacity: 0;

      &:hover {
        opacity: 1;
      }
    }

  }
}
</style>

