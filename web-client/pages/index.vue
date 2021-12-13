<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">

      <div v-if="this.tricks" class="mb-3">
        <div v-for="t in this.tricks">
          {{ t.name }}
          <div>
            <video controls="controls" width="400" height="300" :src="`http://localhost:5000/api/videos/${t.videoName}`"></video>
          </div>
        </div>
      </div>

      <v-stepper v-model="stepNumber">
        <v-stepper-header>
          <v-stepper-step :complete="stepNumber === 1" step="1">Upload video</v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step :complete="stepNumber === 2" step="2">Trick information</v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step :complete="stepNumber === 3" step="3">Confirmation</v-stepper-step>
        </v-stepper-header>

        <v-stepper-items>
          <v-stepper-content step="1">
            <v-card class="mb-12 pa-2">
              <v-file-input accept="video/*" v-model="trickVideo" label="Add a trick video here"></v-file-input>
            </v-card>
            <div class="d-flex justify-space-between">
              <div></div>
              <v-btn color="primary" @click="stepNumber++" :disabled="!trickVideo">Continue >>></v-btn>
            </div>
          </v-stepper-content>

          <v-stepper-content step="2">
            <v-card class="mb-12 pa-2">
              <v-text-field labe="Trick Name" v-model="trickName"></v-text-field>
            </v-card>
            <div class="d-flex justify-space-between">
              <v-btn @click="stepNumber--"><<< Back</v-btn>
              <v-btn color="primary" @click="preparePreview" :disabled="!trickCleanedName">Continue >>></v-btn>
            </div>
          </v-stepper-content>

          <v-stepper-content step="3">
            <v-card class="mb-12 pa-2">
              <p class="mb-2">{{ trickName }} </p>
              <video width="100%" height="300" controls="controls" :src="previewVideoSrc"></video>
            </v-card>
            <div class="d-flex justify-space-between">
              <v-btn @click="stepNumber--"><<< Back</v-btn>
              <v-btn color="success" @click="saveTrick" class="ma-2 white--text">
                <v-icon dark>mdi-cloud-upload</v-icon>&nbsp; Upload
              </v-btn>
            </div>
          </v-stepper-content>
        </v-stepper-items>
      </v-stepper>
    </v-col>
  </v-row>
</template>

<script>
import {mapState, mapActions, mapMutations} from 'vuex';

export default {
  computed: {
    ...mapState('tricks', {
      tricks: state => state.tricks
    }),
    trickCleanedName() {
      return this.trickName.trim();
    }
  },
  data: () => {
    return {
      trickName: '',
      stepNumber: 1,
      trickVideo: null,
      previewVideoSrc: ''
    }
  },
  methods: {
    ...mapMutations('tricks', {
      resetTricks: 'reset'
    }),
    ...mapActions('tricks', ['createTrick']),
    async saveTrick() {

      const form = new FormData();
      form.append('video', this.trickVideo);
      form.append('name', this.trickCleanedName);

      await this.createTrick({ trickFormData: form })
      this.trickName = '';
      this.previewVideoSrc = '';
      URL.revokeObjectURL(this.trickVideo);
      this.trickVideo = null;
      this.stepNumber = 1;
    },
    preparePreview() {
      this.previewVideoSrc = URL.createObjectURL(this.trickVideo);
      this.stepNumber++;
    }
  }
}

</script>
