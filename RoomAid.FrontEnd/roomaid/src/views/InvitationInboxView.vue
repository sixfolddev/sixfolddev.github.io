<template>
<div id = "InvitationInboxView"> <!-- Start inbox container -->
  <v-container fluid>
    <InboxNavBar/>
    <InboxContainer/>
  <div class="messages"> <!-- Start inbox container -->
    <!-- Start message items -->
  </div>
  </v-container>
</div>
</template>

<script>
// @ is an alias to /src
import InboxNavBar from '@/components/InboxNavBar.vue'
import InboxContainer from '@/components/InboxContainer.vue'

export default {
  name: 'InvitationInboxView',
  components: {
    InboxNavBar,
    InboxContainer
  },
  data: () => ({

  }),
  methods: {

  }
}
</script>

<style scoped>

</style>
