<template>
  <v-app>
    <v-main>
      <TaskComponent/>
    </v-main>
  </v-app>
</template>

<script>
import TaskComponent from './components/TaskComponent';

export default {
  name: 'App',

  components: {
    TaskComponent,
  },

  data: () => ({
    //
  }),
};
</script>
