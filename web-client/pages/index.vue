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
import { UserManager, WebStorageStateStore } from 'oidc-client';

export default {
  data: () => ({
    userMgr: null
  }),
  created() {
    if (!process.server){
      this.userMgr = new UserManager({
        authority: "https://localhost:5001",
        client_id: "web-client",
        redirect_uri: "https://localhost:3000/oidc/sign-in-callback.html",
        response_type: "code",
        scope: "openid profile IdentityServerApi role",
        post_logout_redirect_uri: "https://localhost:3000",
        // silent_redirect_uri: "https://localhost:3000/",
        userStore: new WebStorageStateStore({
          store: window.localStorage
        }),
      });

      this.userMgr.getUser().then(user => {
        if (user) {
          console.log("user from storage ", user);
          this.$axios.setToken(`Bearer ${user.access_token}`)
        }
      });
    }
  },
  methods: {
    login() {
      return this.userMgr.signinRedirect();
    },
    api(x) {
      return this.$axios.$get("/api/tricks/" + x)
      .then(msg => console.log(msg));
    },
    logout() {
      return this.userMgr.signoutRedirect();
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
