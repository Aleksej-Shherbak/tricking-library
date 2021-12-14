<template>
  <v-app dark>
    <v-main>
      <v-container fluid>
        <v-app-bar dense  class="mb-3">
          <v-toolbar-title>Trick library</v-toolbar-title>

          <v-spacer></v-spacer>

          <upload-trick />
          <category-form />
        </v-app-bar>
        <Nuxt />
      </v-container>
    </v-main>
  </v-app>
</template>

<script>

export default {

}
</script>
