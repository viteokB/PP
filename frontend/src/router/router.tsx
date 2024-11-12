import { createBrowserRouter, Outlet } from "react-router-dom"
import App from "../App"
import Auth from "../pages/Auth"
import React from "react"
import Header from "../components/Header"
import Footer from "../components/Footer"
import Layout from "../pages/Layout"
import Home from "../pages/Home"

export const router= createBrowserRouter([
  {
    path: "/",
    element: <Layout/>,
    children: [
      {
        index: true,
        element: <Home/>
      },
      {
        path: "/createform",
        element: <div>йооооу</div>
      },
      {
        path: "/profile",
        element: <div>Личный профайл</div>
      },
    ]
  },

  {
    path: "/auth",
    element: <Auth />
  },

])