<template>
  <v-app dark>
    <v-main>
      <v-container fluid>
        <v-app-bar dense class="mb-3">
          <nuxt-link class="text-h5 text--primary mr-2 main-site-header" style="text-decoration: none;" to="/">
            <span class="d-none d-md-flex">Tricking Library</span>
            <span class="d-flex d-md-none">TL</span>
          </nuxt-link>

          <v-spacer></v-spacer>

          <content-creation/>

        </v-app-bar>
        <Nuxt />
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
  import ContentCreation from '../components/content-creation/content-creation';

export default {
  components: {
    ContentCreation
  }
}
</script>

<style scoped>
.main-site-header {
  text-decoration: none;
}
</style>
