<template>
  <div>
    <v-text-field
      label="Comment"
      v-model="content"
      @keydown.ctrl.enter="$emit('send', content)"
      clearable
    />
    <v-btn v-if="$listeners['cancel']" @click="$emit('cancel')">Cancel</v-btn>
    <v-btn @click="$emit('send', content)" :disabled="!content.trim()">{{  label }}</v-btn>
  </div>
</template>

<script>
export default {
  name: "comment-input",
  props: {
    label: {
      type: String,
      default: 'Send'
    }
  },
  data: () => ({
    content: ''
  })
}
</script>

<style scoped>

</style>
