import React from "react"
import { createRoot } from "react-dom/client"
import { Provider } from "react-redux"
import App from "./App"
import { store } from "./app/store"
import "./index.css"
import { createBrowserRouter, RouterProvider } from "react-router-dom"
import Auth from "./pages/Auth"

const router = createBrowserRouter([
  {
    path: "/",
    element: <App></App>
  },
  {
    path: "/createform",
    element: <div>йооооу</div>
  },
  {
    path: "/profile",
    element: <div>Личный профайл</div>
  },
  {
    path: "/auth",
    element: <Auth/>
  },
  {
    path: "/register",
    element: <div>Регистрация</div>
  },
])

const container = document.getElementById("root")
if (container) {
  const root = createRoot(container)

  root.render(
    <React.StrictMode>
      <Provider store={store}>
        <RouterProvider router={router}></RouterProvider>
      </Provider>
    </React.StrictMode>,
  )
} else {
  throw new Error(
    "Root element with ID 'root' was not found in the document. Ensure there is a corresponding HTML element with the ID 'root' in your HTML file.",
  )
}
