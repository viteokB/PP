export default interface IInput {
  type: "text" | "textarea" | "date" | "time" | "checkbox",
  label?: string,
  onClick?: () => void,
}