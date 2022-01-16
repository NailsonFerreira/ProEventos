export class LogUtil {

  public static log(message:string, obj:any = null):void{
    console.log(`${message} ${JSON.stringify(obj!)}`);
  }


}
