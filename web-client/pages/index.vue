<template>
    <v-row justify="center" align="center">
      <v-col cols="12" sm="8" md="6">

        <div v-if="this.tricks" class="mb-3">
          <div v-for="c in this.tricks">
            <v-btn :to="`/trick/${c.id}`">{{ c.name }}</v-btn>
          </div>
        </div>

        <v-spacer></v-spacer>

      </v-col>
    </v-row>
</template>

<script>
import { mapState } from 'vuex';

export default {
  computed: {
    ...mapState('tricks', ['tricks']),
  }
}

</script>
