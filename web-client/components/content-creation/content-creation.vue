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
            @click="activate({ item: item })">
            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </template>

    <v-card v-if="item">
      <v-card-title>
        {{ item.title }}
        <v-spacer></v-spacer>
        <v-btn small color="red" @click="resetForm">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-card-title>
      <v-card-text>
      <component :is="item.component" v-bind="{resetForm}"></component>
      </v-card-text>
    </v-card>

  </v-dialog>
</template>

<script>
import TrickForm from './trick-form';
import SubmissionSteps from './submission-steps';
import DifficultyForm from './difficulty-form';
import CategoryForm from './category-form';
import {mapMutations} from "vuex";

const submissionStepsComponentTitle = 'Submission';

export default {
  name: "content-creation",
  components: {
    TrickForm,
    SubmissionSteps,
    DifficultyForm,
    CategoryForm
  },
  data: () => ({
    item: null,
    isActive: false,
  }),
  computed: {
    menuItems() {
      return [
        { component: TrickForm, title: 'Trick' },
        { component: SubmissionSteps, title: submissionStepsComponentTitle },
        { component: DifficultyForm, title: 'Difficulty' },
        { component: CategoryForm, title: 'Category' },
      ]
    }
  },
  methods: {
    ...mapMutations('upload-video', ['disposeVideo']),
    activate({ item }) {
      this.isActive = true;
      this.item = item;
    },
    resetForm() {
      this.isActive = false;
      if (this.item.title === submissionStepsComponentTitle) {
        this.disposeVideo();
      }
      this.item = null;
    }
  },
}
</script>
