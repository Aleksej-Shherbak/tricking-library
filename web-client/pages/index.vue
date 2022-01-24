<template>
  <div>
    <div>
      <v-btn @click="login">Login</v-btn>
      <v-btn @click="logout">Logout</v-btn>
      <v-btn @click="api('test')">Api test</v-btn>
      <v-btn @click="api('mod')">Api mod test</v-btn>
    </div>

    <div v-for="section in sections" :key="section.title">
      <div  class="d-flex flex-column align-center">
        <h3 class="text-h5 mb-4">{{ section.title }}</h3>
        <div>
          <v-btn class="mx-1" v-for="item in section.collection" :key="item.id" :to="section.routeFactory(item.id)">{{ item.name }}</v-btn>
        </div>
      </div>
      <v-divider class="my-5"></v-divider>
    </div>

  </div>


</template>

<script>
import { mapState } from 'vuex';

export default {
  created() {
    if (!process.server){
      this.$auth.getUser().then(user => {
        if (user) {
          console.log("user from storage ", user);
          this.$axios.setToken(`Bearer ${user.access_token}`)
        }
      });
    }
  },
  methods: {
    login() {
      return this.$auth.signinRedirect();
    },
    api(x) {
      return this.$axios.$get("/api/tricks/" + x)
      .then(msg => console.log(msg));
    },
    logout() {
      return this.$auth.signoutRedirect();
    }
  },
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
