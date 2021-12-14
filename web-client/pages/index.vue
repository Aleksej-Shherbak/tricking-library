<template>
    <v-row justify="center" align="center">
      <v-col cols="12" sm="8" md="6">

        <div v-if="this.tricks" class="mb-3">
          <div v-for="t in this.tricks">
            {{ t.name }}
            <div>
              <video controls="controls" width="400" height="300"
                     :src="`http://localhost:5000/api/videos/${t.videoName}`"></video>
            </div>
          </div>
        </div>
      </v-col>
    </v-row>
</template>

<script>
import { mapState } from 'vuex';

export default {
  computed: {
    ...mapState('tricks', ['tricks']),
  }
}

</script>
