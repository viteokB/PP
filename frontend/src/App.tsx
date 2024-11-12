import "./App.css"
import Header from "./components/Header"
import Footer from "./components/Footer"
import Home from "./pages/Home"
import { Outlet, RouterProvider } from "react-router-dom"
import { router } from "./router/router"
const App = () => {

  return (
    <RouterProvider router={router}>
    </RouterProvider>

  )
}

export default App
