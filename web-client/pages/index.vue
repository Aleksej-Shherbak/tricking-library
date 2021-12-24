<template>
  <div>
    <div v-for="section in sections">
      <div  class="d-flex flex-column align-center">
        <h3 class="text-h5 mb-4">{{ section.title }}</h3>
        <div>
          <v-btn class="mx-1" v-for="item in section.collection" :to="section.routeFactory(item.id)">{{ item.name }}</v-btn>
        </div>
      </div>
      <v-divider class="my-5"></v-divider>
    </div>

  </div>


</template>

<script>
import { mapState } from 'vuex';

export default {
  computed: {
    ...mapState('tricks', ['tricks', 'categories', 'difficulties']),
    sections() {
      return [
        { collection: this.tricks, title: 'Tricks', routeFactory: (id) => `/trick/${id}` },
        { collection: this.categories, title: 'Categories', routeFactory: (id) => `/category/${id}` },
        { collection: this.difficulties, title: 'Difficulties', routeFactory: (id) => `/difficulty/${id}` },
      ];
    }
  }
}

</script>
