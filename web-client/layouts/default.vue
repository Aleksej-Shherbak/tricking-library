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

          <v-btn class="mx-1" v-if="this.moderator" depressed to="/moderation">Moderation</v-btn>

          <v-skeleton-loader class="mx-1" :loading="isLoading" transition="fade-transition" type="button">
            <content-creation/>
          </v-skeleton-loader>

          <v-skeleton-loader
            :loading="isLoading"
            transition="fade-transition"
            type="button">
            <v-btn v-if="this.authenticated" outlined depressed>
              <v-icon left>mdi-account-circle</v-icon> Profile
            </v-btn>
            <v-btn v-else outlined depressed @click="$auth.signinRedirect();">
              <v-icon left>mdi-account-circle-outline</v-icon> sign in
            </v-btn>
          </v-skeleton-loader>
          <v-btn v-if="this.authenticated" depressed @click="$auth.signoutRedirect()">Logout</v-btn>

        </v-app-bar>
        <Nuxt />
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
  import ContentCreation from '../components/content-creation/content-creation';
  import {mapGetters, mapState} from "vuex";

export default {
  components: {
    ContentCreation
  },
  computed: {
    ...mapState('auth', ['isLoading']),
    ...mapGetters('auth', ['authenticated', 'moderator']),
  }
}
</script>

<style scoped>
.main-site-header {
  text-decoration: none;
}
</style>
