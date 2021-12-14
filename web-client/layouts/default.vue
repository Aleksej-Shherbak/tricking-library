<template>
  <v-app dark>
    <v-main>
      <v-container fluid>

        <v-app-bar dense  class="mb-3">
          <v-toolbar-title>Trick library</v-toolbar-title>
          <v-spacer></v-spacer>

          <v-btn depressed @click="toggleUploadTrickActivity">
            Upload trick
          </v-btn>

          <v-btn depressed @click="toggleCategoryDialogActivity">
            Create categpry
          </v-btn>

        </v-app-bar>
        <Nuxt />
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import { mapMutations } from 'vuex';

export default {
  methods: {
    ...mapMutations('tricks', ['toggleUploadTrickActivity']),
    ...mapMutations('category', ['toggleCategoryDialogActivity'])
  }

}
</script>
