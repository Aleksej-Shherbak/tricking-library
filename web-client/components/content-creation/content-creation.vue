<template>
  <v-dialog :value="isActive" persistent width="700" >
    <template v-slot:activator="{on}">
      <v-menu offset-y>
        <template v-slot:activator="{ on, attrs }">
          <v-btn depressed v-bind="attrs" v-on="on">
            Create
          </v-btn>
        </template>
        <v-list>
          <v-list-item v-for="(item, i) in menuItems" :key="`ccd-menu-${i}`"
            @click="activate({ component: item.component })">

            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </template>

    <div v-if="component" class="content-creation-dialog">
      <component :is="component" v-bind="{resetForm}"></component>
      <v-btn absolute fab small top right color="red" @click="reset">
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </div>

  </v-dialog>
</template>

<script>
import TrickForm from './trick-form';
import SubmissionForm from './submission-form';
import DifficultyForm from './difficulty-form';
import CategoryForm from './category-form';

export default {
  name: "content-creation",
  components: {
    TrickForm,
    SubmissionForm,
    DifficultyForm,
    CategoryForm
  },
  data: () => ({
    component: null,
    isActive: false,
  }),
  computed: {
    menuItems() {
      return [
        { component: TrickForm, title: "Trick" },
        { component: SubmissionForm, title: "Submission" },
        { component: DifficultyForm, title: "Difficulty" },
        { component: CategoryForm, title: "Category" },
      ]
    }
  },
  methods: {
    activate({ component }) {
      this.isActive = true;
      this.component = component
    },
    resetForm() {
      this.isActive = false;
      this.component = null;
    }
  },
}
</script>

<style scoped>
.content-creation-dialog {
  position: relative;
  margin-top: 25px;
}
</style>
