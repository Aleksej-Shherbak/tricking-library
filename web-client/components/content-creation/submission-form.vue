<template>
  <v-stepper v-model="stepNumber">
    <v-stepper-header>
      <v-stepper-step :complete="stepNumber === 1" step="1">Select trick</v-stepper-step>

      <v-divider></v-divider>

      <v-stepper-step :complete="stepNumber === 2" step="2">Upload video</v-stepper-step>

      <v-divider></v-divider>

      <v-stepper-step :complete="stepNumber === 3" step="3">Add information</v-stepper-step>

      <v-divider></v-divider>

      <v-stepper-step :complete="stepNumber === 4" step="4">Review</v-stepper-step>
    </v-stepper-header>

    <v-stepper-items>
      <v-stepper-content step="1">
        <v-card class="mb-12 pa-2">
          <v-select
            class="trick-select"
            :items="trickItems"
            label="Chose trick type"
            v-model="trickId"
            outlined
          ></v-select>
        </v-card>
        <div class="d-flex justify-space-between">
          <v-btn color="primary" @click="stepNumber++" :disabled="!trickId">Continue >>></v-btn>
        </div>
      </v-stepper-content>

      <v-stepper-content step="2">
        <v-card class="mb-12 pa-2">
          <v-file-input accept="video/*" @change="this.setVideo" label="Add a trick video here"></v-file-input>
        </v-card>
        <div class="d-flex justify-space-between">
          <v-btn @click="stepNumber--"><<< Back</v-btn>
          <v-btn color="primary" @click="stepNumber++" :disabled="!this.video">Continue >>></v-btn>
        </div>
      </v-stepper-content>

      <v-stepper-content step="3">
        <v-card class="mb-12 pa-2">
          <v-text-field labe="Submission Name" v-model="name"></v-text-field>
        </v-card>
        <div class="d-flex justify-space-between">
          <v-btn @click="stepNumber--"><<< Back</v-btn>
          <v-btn color="primary" @click="stepNumber++" :disabled="!cleanedName">Continue >>></v-btn>
        </div>
      </v-stepper-content>

      <v-stepper-content step="4">
        <v-card class="mb-12 pa-2">
          <p class="mb-2">{{ name }} </p>
          <video-player :video="this.previewUrl"/>
        </v-card>
        <div class="d-flex justify-space-between">
          <v-btn @click="stepNumber--"><<< Back</v-btn>
          <v-btn color="success" @click="save" class="ma-2 white--text">
            <v-icon dark>mdi-cloud-upload</v-icon>&nbsp; Upload
          </v-btn>
        </div>
      </v-stepper-content>
    </v-stepper-items>
  </v-stepper>
</template>

<script>
import { mapState, mapActions, mapMutations, mapGetters } from 'vuex';
import VideoPlayer from "../../components/video-player";

export default {
  name: "submission-form",
  props: {
    resetForm: {
      required: true,
      type: Function
    }
  },
  components: {
    VideoPlayer
  },
  computed: {
    ...mapState('tricks', ['tricks']),
    ...mapGetters('tricks', ['trickItems']),
    ...mapGetters('upload-video', ['video', 'previewUrl']),
    cleanedName() {
      return this.name.trim();
    },
  },
  data: () => {
    return {
      name: '',
      stepNumber: 1,
      trickId: null
    }
  },
  methods: {
    ...mapActions('submissions', ['createSubmission']),
    ...mapMutations('upload-video', ['setVideo', 'dispose']),
    async save() {
      const form = new FormData();
      form.append('video', this.video);
      form.append('name', this.cleanedName);
      form.append('trickId', this.trickId);

      await this.createSubmission({ submissionFormData: form});

      this.dispose();
      this.resetForm();
    },
  }
}
</script>

<style scoped>
  .trick-select {
    max-width: 400px;
  }
</style>
