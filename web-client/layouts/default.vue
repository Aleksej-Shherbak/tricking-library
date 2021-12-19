<template>
  <v-app dark>
    <v-main>
      <v-container fluid>
        <v-app-bar dense class="mb-3">
          <v-toolbar-title>Trick library</v-toolbar-title>

          <v-spacer></v-spacer>

          <trick-form />
          <submission-form />
          <category-form/>
          <difficulty-form/>

        </v-app-bar>
        <Nuxt />
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
  import TrickForm from '../components/forms/trick-form';
  import SubmissionForm from '../components/forms/submission-form';
  import DifficultyForm from '../components/forms/difficulty-form';
  import CategoryForm from '../components/forms/category-form';

export default {
  components: {
    TrickForm,
    SubmissionForm,
    DifficultyForm,
    CategoryForm
  }
}
</script>
