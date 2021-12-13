<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
      <v-card class="logo py-4 d-flex justify-center">
        <NuxtLogo />
        <VuetifyLogo />
      </v-card>
      <div v-if="this.tricks">
        <p v-for="t in this.tricks">
          {{ t.name }}
        </p>
      </div>

      <div>
        <v-text-field labe="Trick Name" v-model="trickName"></v-text-field>
        <v-btn @click="saveTrick">Save trick</v-btn>
      </div>

      <div>
        {{ message }}
      </div>

      <v-btn @click="reset">Reset message</v-btn>
      <v-btn @click="reset">Reset tricks</v-btn>

    </v-col>
  </v-row>
</template>

<script>
import { mapState, mapActions, mapMutations } from 'vuex';

export default {

  computed: {
    ...mapState({
      message: state => state.message
    }),
    ...mapState('tricks', {
      tricks: state => state.tricks
    })
  },
  data: () => {
    return {
      trickName: ''
    }
  },
  methods: {
      ...mapMutations('tricks', [
      'reset'
    ]),
    ...mapMutations({
      resetTricks: 'reset'
    }),
    ...mapActions('tricks', ['createTrick']),
    async saveTrick() {
      await this.createTrick( { trick: { name: this.trickName } })
      this.trickName = ''
    }
  }
 /* async fetch () {
    await this.$store.dispatch('fetchMessage')
  }*/
  /*asyncData(payload){
    return payload.$axios.get('http://localhost:5000/api/home')
    .then(({ data }) => {
      return { message: data }
    });
  },*/
}

</script>
