<template>
  <v-dialog @click:outside="closeTrickDialog" :value="isUploadPopupOpened">

    <template v-slot:activator="{ on }">
      <v-btn depressed @click="toggleUploadTrickActivity">
        Upload trick
      </v-btn>
    </template>

    <v-stepper v-model="stepNumber">
      <v-stepper-header>
        <v-stepper-step :complete="stepNumber === 1" step="1">Select category</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step :complete="stepNumber === 2" step="2">Upload video</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step :complete="stepNumber === 3" step="3">Trick information</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step :complete="stepNumber === 4" step="4">Review</v-stepper-step>
      </v-stepper-header>

      <v-stepper-items>
        <v-stepper-content step="1">
          <v-card class="mb-12 pa-2">
            <v-select
              class="category-select"
              :items="mapCategorySelectItems"
              label="Chose trick type"
              v-model="categoryId"
              outlined
            ></v-select>
          </v-card>
          <div class="d-flex justify-space-between">
            <v-btn @click="closeTrickDialog"> <v-icon>mdi-close</v-icon>&nbsp; Close</v-btn>
            <v-btn color="primary" @click="stepNumber++" :disabled="!categoryId">Continue >>></v-btn>
          </div>
        </v-stepper-content>

        <v-stepper-content step="2">
          <v-card class="mb-12 pa-2">
            <v-file-input accept="video/*" v-model="trickVideo" label="Add a trick video here"></v-file-input>
          </v-card>
          <div class="d-flex justify-space-between">
            <v-btn @click="stepNumber--"><<< Back</v-btn>
            <v-btn color="primary" @click="stepNumber++" :disabled="!trickVideo">Continue >>></v-btn>
          </div>
        </v-stepper-content>

        <v-stepper-content step="3">
          <v-card class="mb-12 pa-2">
            <v-text-field labe="Trick Name" v-model="trickName"></v-text-field>
          </v-card>
          <div class="d-flex justify-space-between">
            <v-btn @click="stepNumber--"><<< Back</v-btn>
            <v-btn color="primary" @click="preparePreview" :disabled="!trickCleanedName">Continue >>></v-btn>
          </div>
        </v-stepper-content>

        <v-stepper-content step="4">
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
  </v-dialog>
</template>

<script>
import {mapState, mapActions, mapMutations} from 'vuex';

export default {
  name: "upload-trick",
  computed: {
    ...mapState('tricks', ['tricks', 'isUploadPopupOpened']),
    ...mapState('categories', ['categories']),
    trickCleanedName() {
      return this.trickName.trim();
    },
    mapCategorySelectItems() {
      return this.categories.map(({ name, id }) => {
        return {
          text: name,
          value: id,
        };
      });
    }
  },
  data: () => {
    return {
      trickName: '',
      stepNumber: 1,
      trickVideo: null,
      previewVideoSrc: '',
      categoryId: null
    }
  },
  methods: {
    ...mapActions('tricks', ['createTrick']),
    ...mapMutations('tricks', ['toggleUploadTrickActivity']),
    async saveTrick() {

      const form = new FormData();
      form.append('video', this.trickVideo);
      form.append('name', this.trickCleanedName);

      await this.createTrick({trickFormData: form})
      this.closeTrickDialog();
    },
    closeTrickDialog() {
      this.toggleUploadTrickActivity();
      this.trickName = '';
      this.previewVideoSrc = '';
      this.categoryId = null;
      if (this.trickVideo) {
        URL.revokeObjectURL(this.trickVideo);
      }
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

<style scoped>
  .category-select {
    max-width: 400px;
  }
</style>
