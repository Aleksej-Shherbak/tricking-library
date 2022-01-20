<template>
  <div>
    <div>
      <v-btn @click="login">Login</v-btn>
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
        authority: "http://localhost:5000",
        client_id: "web-client",
        redirect_uri: "http://localhost:3000",
        response_type: "code",
        scope: "openid profile",
        post_logout_redirect_uri: "http://localhost:3000z ",
        // silent_redirect_uri: "http://localhost:3000/",
        userStore: new WebStorageStateStore({
          store: window.localStorage
        }),
      });

      const { code, scope, session_state, state } = this.$route.query;

      if (code && scope && session_state && state) {
        // We are ready to finish the OAuth flow with the help of the following method's call
        this.userMgr.signinRedirectCallback()
        .then((user) => {
          console.log(user)
          this.$router.push('/')
        })
      }

    }
  },
  methods: {
    login() {
      return this.userMgr.signinRedirect();
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
